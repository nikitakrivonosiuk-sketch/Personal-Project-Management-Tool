import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CardItem } from '../list/card/card-item';
import { ListItem } from '../list/list-item';

@Component({
  selector: 'app-card-modal',
  imports: [FormsModule],
  templateUrl: './card-modal.html',
  styleUrl: './card-modal.css',
})
export class CardModal implements OnChanges {
  @Input() cardData?: CardItem;
  @Input() currentListId?: string;
  @Input() lists?: ListItem[];
  @Output() isOpened = new EventEmitter<boolean>();
  @Output() savedCard = new EventEmitter<Partial<CardItem>>();

  cardModel: Partial<CardItem> = {
    title: '',
    listId: '',
    description: '',
    priority: 'Low',
  };

  ngOnChanges(changes: SimpleChanges){
    if (this.cardData){
      this.cardModel = {...this.cardData}

      if (this.cardModel.dueDate) {
      this.cardModel.dueDate = this.cardModel.dueDate.toString().split('T')[0];
    }
    }
    else if(this.currentListId) {
      this.cardModel = {
        title: '',
        description: '',
        priority: 'Low',
        listId: this.currentListId
      };
    }

    console.log(this.cardData)
  }

  onSave(){
    if(!this.cardModel.title?.trim()){
      alert("Please enter the card name.")
      return;
    }
    if(!this.cardModel.dueDate){
      alert("Please enter the date.")
      return;
    }
    if(!this.cardModel.priority){
      alert("Please enter the card name.")
      return;
    }
    if(this.cardModel.listId === ''){
      alert("Please choose the list.")
      return;
    }
    this.savedCard.emit(this.cardModel);
  }

  close(){
    this.cardModel.description = '';

    this.isOpened.emit(false);
  }
}
