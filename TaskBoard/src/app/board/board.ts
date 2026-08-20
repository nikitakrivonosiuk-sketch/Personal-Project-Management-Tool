import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ListItem } from './list/list-item';
import { List } from "./list/list";
import { CardModal } from "./card-modal/card-modal";
import { CardActionEvent, CardItem } from './list/card/card-item';
import { CardViewModal } from "./card-view-modal/card-view-modal";

@Component({
  selector: 'app-board',
  imports: [List, FormsModule, CardModal, CardViewModal],
  templateUrl: './board.html',
  styleUrl: './board.css',
})
export class Board {
  boardLists: ListItem[] = [{
    id: '1',
    title: 'To Do',
    cards: [{
      id: Math.random().toString(),
      listId: '1',
      title: 'Create an app',
      description: 'Task descriptions should be unambiguous, accurate, factual, comprehensible, correct.',
      dueDate: '2026-07-30',
      priority: 'medium',
      status: 'todo',
      },
      {
      id: Math.random().toString(),
      listId: '1',
      title: 'Card Name',
      description: 'Task descriptions should be unambiguous, accurate, factual, comprehensible, correct.',
      dueDate: '2026-07-29',
      priority: 'low',
      status: 'in progress',
      }
    ],
  },
  
  { id: '2',
    title: 'In Progress',
    cards: [{
      id: Math.random().toString(),
      listId: '2',
      title: 'Fixing bugs',
      description: 'Task descriptions should be unambiguous, accurate, factual, comprehensible, correct.',
      dueDate: '2026-07-30',
      priority: 'medium',
      status: 'todo',
    }],
  }]

  isAddingList = false;
  isViewCardModal = false;
  isCardModal = false;
  newListTitle: string = '';
  isEditingCardData?: CardItem;
  listId: string = '';
  isViewingCard?: CardItem;

  onAddListClick(){
    const newList:ListItem = {
      id: Math.random().toString(),
      title: this.newListTitle,
      cards: []
    };

    console.log(newList.id);

    this.boardLists.push(newList);

    this.newListTitle = '';

    this.isAddingList = false;
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
        this.MoveCard(event.card.listId, event.card);
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

  // ViewCard(card: CardItem){
  //   this.isViewCardModal = true;
  //   this.isViewingCard = card;
  // }

  EditCard(card: CardItem){
      this.isEditingCardData = card;
      this.isCardModal = true;
  }

  MoveCard(desiredListId: string, card: CardItem){
    this.boardLists = this.boardLists.map(list => {
      let updatedCards = list.cards.filter(x => x.id !== card.id);

      if (desiredListId === list.id){
        updatedCards = [...updatedCards, card];
      }

      return {...list, cards: updatedCards }
    });
  }

  DeleteCard(listId: string, cardId: string){
    const targetList = this.boardLists.find(x => x.id === listId);

    if (!targetList){
      console.log("List was not found.");
      return;
    }

    //Delete from DB
    targetList.cards = targetList.cards.filter(x => x.id !== cardId);
  }

  SaveCardToList(cardData: Partial<CardItem>){
    const targetListId = cardData.listId || this.listId;

    if(this.isEditingCardData){
      const oldListId = this.isEditingCardData.listId;

      if (oldListId === targetListId){
        const targetList = this.boardLists.find(list => list.id === targetListId);

        if (targetList){
          targetList.cards = targetList.cards.map(x => x.id === cardData.id ? (cardData as CardItem) : x);
        }
        else {
          console.error("List was not found.");
        }
      }
      else {
        // Removing previous card from list
        this.boardLists.forEach(list =>list.cards = list.cards.filter(x => x.id !== cardData.id));

        const targetList = this.boardLists.find(x => x.id === targetListId);

        if (targetList){
          targetList.cards = [...targetList.cards, cardData as CardItem];
        }
        else {
          console.error("List was not found.")
        }
      }
    }
    else {
      const newCard: CardItem = {
        id: Math.random().toString(),
        listId: targetListId,
        title: cardData.title || 'Untitled',
        description: cardData.description || '',
        dueDate: cardData.dueDate,
        priority: cardData.priority || 'low',
        status: cardData.status || 'todo',
      };
  
      // Find proper list in db
      const targetList = this.boardLists.find(list => list.id === targetListId);
  
      if (targetList){
        targetList.cards.push(newCard);
      }
      else {
        console.error("List is not found.");
      }
      // Save to db
  
      this.isCardModal = false;
      this.listId = '';
    }

    this.isEditingCardData = undefined;
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

  deleteList(listId: string){
    this.boardLists = this.boardLists.filter(x => x.id !== listId);
  }
}
