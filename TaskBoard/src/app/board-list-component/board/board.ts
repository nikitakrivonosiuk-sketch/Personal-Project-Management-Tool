import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ListItem } from './list/list-item';
import { List } from "./list/list";
import { CardModal } from "./card-modal/card-modal";
import { CardActionEvent, CardItem, TaskPriority } from './list/card/card-item';
import { CardViewModal } from "./card-view-modal/card-view-modal";
import { BoardService } from './board-service';
import { BoardListDto, CardDto } from './board-model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-board',
  imports: [List, FormsModule, CardModal, CardViewModal],
  templateUrl: './board.html',
  styleUrl: './board.css',
})
export class Board implements OnInit {

  private boardService = inject(BoardService);
  private route = inject(ActivatedRoute);

  boardLists = signal<ListItem[]>([]);

  isAddingList = false;
  isViewCardModal = false;
  isCardModal = false;
  newListTitle: string = '';
  isEditingCardData?: CardItem;
  listId: string = '';
  isViewingCard?: CardItem;

  ngOnInit(){
    const boardId = this.route.snapshot.paramMap.get('id');

    if (boardId){
      this.boardService.getBoardData(boardId).subscribe({
        next: (data) => {
          this.boardLists.set(data.boardLists.map(list => ({
          id: list.id,
          boardId: list.boardId,
          title: list.title,
          position: list.position,
          cards: list.cards.map(c => ({
            id: c.id,
            listId: c.boardListId,
            title: c.title,
            description: c.description || "",
            dueDate: c.dueDate || "",
            priority: (c.priority || 'Low') as TaskPriority,
          }))
        })));
        },
        error: err => console.error("Бекенд ліг або CORS не пускає:", err),
      });
    }
  }

  onAddListClick(){
    const currentBoardId = this.route.snapshot.paramMap.get('id');

    if (!currentBoardId) {
      console.error("Id is missing in routing.");
      return;
    }

    const newList = {
      title: this.newListTitle,
      boardId: currentBoardId,
    };

    this.boardService.createList(newList).subscribe({
      next: (createdListFromDb : BoardListDto) => {
        this.boardLists.update(lists => [...lists, {...createdListFromDb, cards: [] } as unknown as ListItem]);

        this.newListTitle = '';
        this.isAddingList = false;
        this.boardService.logsUpdated.next();
      },
      error: (err: any) => console.error("Something bad with backend", err),
    });
  }

  onHandleCard(event: CardActionEvent){
    switch(event.cardAction){
      case 'edit':
        //edit card
        this.EditCard(event.card);
        break;
      case 'delete':
        //delete card
        this.DeleteCard(event.card.listId, event.card.id);
        break;
      case 'move':
        //moving card to another list
        this.MoveCard(event.card, event.desiredListId!);
        break;
      case 'view':
        // Just view a card lol.
        this.isViewCardModal = true;
        this.isViewingCard = event.card;
        break;
      default:
        console.error("Incorrect action.")
        break;
    }
  }

  EditCard(card: CardItem){
      this.isEditingCardData = card;
      this.isCardModal = true;
      this.isViewCardModal = false;
  }

  MoveCard(card: CardItem, desiredListId: string){
    const updatedCard = {...card, boardListId: desiredListId}

    this.boardService.updateCard(updatedCard.id, updatedCard).subscribe({
      next: (updatedCardFromDb) => {
        this.boardLists.update(lists => lists.map(list => {
          const updatedCards = list.cards.filter(c => c.id !== card.id);

          if (desiredListId === list.id){
            const mappedCard: CardItem = {
              ...card,
              listId: desiredListId
            };

            const newCards = [...updatedCards, mappedCard];

            // Sorting this array by datetime
            newCards.sort((x, y) => {
              const dateX = x.dueDate ? new Date(x.dueDate).getTime() : Infinity;
              const dateY = y.dueDate ? new Date(y.dueDate).getTime() : Infinity;
              return (dateX - dateY);
            });
            return {...list, cards: newCards};
          };
          return { ...list, cards: updatedCards };
        }));
        this.boardService.logsUpdated.next();
      },
      error: (err) => console.error("Smthing wrong with backend or couldnt find card in DB.", err),
    });
  };

  DeleteCard(listId: string, cardId: string){
    this.boardService.deleteCard(cardId).subscribe({
      next: () => {
        this.boardLists.update(lists => 
            lists.map(list => ({
                ...list,
                cards: list.cards.filter(c => c.id !== cardId)
            }))
        );
        this.boardService.logsUpdated.next();
      },
      error: (err: any) => console.error("Smth wrong with backend.", err),
    })
  }

  SaveCardToList(cardData: Partial<CardItem>){
    const targetListId = cardData.listId || this.listId;

    // If updating an existing card:
      if (this.isEditingCardData){
        
        const newCardData = {
          title: cardData.title,
          description: cardData.description,
          dueDate: cardData.dueDate,
          priority: cardData.priority,
          boardListId: targetListId,          
        }

        this.boardService.updateCard(this.isEditingCardData.id, newCardData).subscribe({
          next: (updatedCard: CardDto) => {

            const mappedCard: CardItem = {
              id: updatedCard.id,
              title: updatedCard.title,
              description: updatedCard.description || '',
              dueDate: updatedCard.dueDate || '',
              priority: updatedCard.priority,
              listId: updatedCard.boardListId 
            };

            this.boardLists.update(lists => {
              let updatedLists = [...lists];

              updatedLists = updatedLists.map(list => ({
                ...list,
                cards: list.cards.filter(cards => cards.id !== cardData.id)
              }));

              updatedLists = updatedLists.map(list => {
                if (list.id === targetListId){
                  const newCardsArray = [...list.cards, mappedCard];  

                  // Sorting this array by datetime
                  newCardsArray.sort((x, y) => {
                    const dateX = x.dueDate ? new Date(x.dueDate).getTime() : Infinity;
                    const dateY = y.dueDate ? new Date(y.dueDate).getTime() : Infinity;

                    return (dateX - dateY);
                  });

                  return {...list, cards: newCardsArray};
                }
                return list;
              });
              return updatedLists;
            });

            this.boardService.logsUpdated.next();
            this.isEditingCardData = undefined;
            this.listId = '';
            this.isCardModal = false;
          },
          error: (err: any) => console.error("Smth wrong with backend.") 
        });
      }
      
      // If creating new 
      else {
        const newCardData = {
          title: cardData.title,
          description: cardData.description || '',
          boardListId: targetListId,
          dueDate: cardData.dueDate || '',
          priority: cardData.priority || 'Low',
        }

        this.boardService.createCard(newCardData).subscribe({
          next: (savedCardFromDb : CardDto) => {

            const mappedCard: CardItem = {
              id: savedCardFromDb.id,
              title: savedCardFromDb.title,
              description: savedCardFromDb.description || '',
              dueDate: savedCardFromDb.dueDate || '',
              priority: savedCardFromDb.priority,
              listId: savedCardFromDb.boardListId
            };

            this.boardLists.update(lists => lists.map(list => {

              if (list.id === targetListId){
                const newCardsArray = [...list.cards, mappedCard];  

                // Sorting this array by datetime

                newCardsArray.sort((x, y) => {
                  const dateX = x.dueDate ? new Date(x.dueDate).getDate() : Infinity;
                  const dateY = y.dueDate ? new Date(y.dueDate).getDate() : Infinity;

                  return (dateX - dateY);
                });
                return {...list, cards: newCardsArray};
              }
              return list;
            }))

            this.boardService.logsUpdated.next();
            this.isCardModal = false;
            this.listId = ''
          },
          error: (err : any) => console.error("Something went wrong with backend:", err),
        });
      }
  }

  openCardModel(listId: string){
    this.isCardModal = true;
    this.listId = listId;
  }

  closeCardModel(){
    this.isEditingCardData = undefined;
    this.listId = '';
    this.isCardModal = false;
    this.isViewCardModal = false;
  }

  updateList(listData: ListItem){
    const newList = {
      id: listData.id,
      title: listData.title,
      position: listData.position,
    }

    this.boardService.updateList(newList.id, newList).subscribe({
      next: (updatedList: BoardListDto) => {
        this.boardLists.update(lists => lists.map(list => {
          if (list.id === updatedList.id){
            return {...list, title: updatedList.title, position: updatedList.position};
          }
          return list;
        }));
        this.boardService.logsUpdated.next();
      },
      error: (err: any) => console.error("Smthing wrong with backend or invalid id.", err),
    });
  }

  deleteList(listId: string){
    this.boardService.deleteList(listId).subscribe({
      next: () => {
        this.boardLists.update(lists => lists.filter(list => list.id !== listId));
        this.boardService.logsUpdated.next();
      },
      error: (err) => console.error("Something bad with backend", err),
    });
  }
}
