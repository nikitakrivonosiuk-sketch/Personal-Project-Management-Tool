import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ListItem } from '../../models/list-model';
import { List } from '../list/list';
import { CardModal } from "../card-modal/card-modal";
import { CardActionEvent, CardItem } from '../../models/card.models';
import { CardViewModal } from "../card-view-modal/card-view-modal";
import { BoardService } from '../../services/board-service';
import { CreateCardRequestDto } from '../../models/board-models';
import { ActivatedRoute } from '@angular/router';
import { ToastService } from '../../services/toast.service/toast.service';
import { Store } from '@ngrx/store';
import { selectBoardList } from '../../state/board.selectors';
import { BoardActions } from '../../state/board.actions';

@Component({
  selector: 'app-board',
  imports: [List, FormsModule, CardModal, CardViewModal],
  templateUrl: './board.html',
  styleUrl: './board.css',
})
export class Board implements OnInit {

  private boardService = inject(BoardService);
  private route = inject(ActivatedRoute);
  toaster = inject(ToastService);
  store = inject(Store);

  boardLists = this.store.selectSignal(selectBoardList);

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
      this.store.dispatch(BoardActions.loadBoard({boardId: boardId}));
    }
  }

  ngOnDestroy() {
    this.boardService.activeBoardId.set(null);
  }

  onAddListClick(){
    const currentBoardId = this.route.snapshot.paramMap.get('id');

    if (!currentBoardId) {
      this.toaster.error("Current board was failed to load.")
      return;
    }

    this.store.dispatch(BoardActions.createList({list: {title: this.newListTitle, boardId: currentBoardId}}))

    this.newListTitle = '';
    this.isAddingList = false
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
        this.MoveCard(event.card, event.desiredListId);
        break;
      case 'view':
        // Just view a card.
        this.isViewCardModal = true;
        this.isViewingCard = event.card;
        break;
      default:
        this.toaster
        break;
    }
  }

  EditCard(card: CardItem){
      this.isEditingCardData = card;
      this.isCardModal = true;
      this.isViewCardModal = false;
  }

  MoveCard(card: CardItem, desiredListId?: string){
    const movedCard: CardItem = {
      ...card,
      listId: desiredListId || card.listId,
    }

    this.store.dispatch(BoardActions.moveCard({card: movedCard}));
  };

  DeleteCard(listId: string, cardId: string){
    this.store.dispatch(BoardActions.deleteCard({listId: listId, cardId: cardId}));
  }

  SaveCardToList(cardData: Partial<CardItem>){
    const targetListId = cardData.listId || this.listId;

    // If updating an existing card:
      if (this.isEditingCardData){
        
        const newCardData: CardItem = {
          id: this.isEditingCardData.id!,
          title: cardData.title!,
          description: cardData.description! || '',
          dueDate: cardData.dueDate!,
          priority: cardData.priority!,
          listId: targetListId!,          
        }

        this.store.dispatch(BoardActions.editCard({card: newCardData}));

        this.isEditingCardData = undefined;
      }
      
      // If creating new 
      else {
        const newCard: CreateCardRequestDto = {
          title: cardData.title!,
          description: cardData.description!,
          dueDate: cardData.dueDate!,
          boardListId: targetListId!,
          priority: cardData.priority!
        }

        this.store.dispatch(BoardActions.createCard({card: newCard}));
      }

      this.listId = '';
      this.isCardModal = false;
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
    this.store.dispatch(BoardActions.renameList({list: listData}))
  }

  deleteList(listId: string){
    this.store.dispatch(BoardActions.deleteList({listId}));
  }
}
