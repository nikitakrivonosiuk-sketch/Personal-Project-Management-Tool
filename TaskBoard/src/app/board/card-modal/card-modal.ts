import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CardItem } from '../list/card/card-item';
import { ListItem } from '../list/list-item';

@Component({
  selector: 'app-card-modal',
  imports: [FormsModule],
  templateUrl: './card-modal.html',
  styleUrl: './card-modal.css',
})
export class CardModal implements OnInit {
  @Input() cardData?: CardItem;
  @Input() currentListId?: string;
  @Input() lists?: ListItem[];
  @Output() isOpened = new EventEmitter<boolean>();
  @Output() savedCard = new EventEmitter<Partial<CardItem>>();

  cardModel: Partial<CardItem> = {
    title: '',
    listId: this.currentListId || '',
    description: '',
    priority: 'low',
    status: 'todo',
  };

  ngOnInit(){
    if (this.cardData){
      this.cardModel = {...this.cardData}
    }
    else if(this.currentListId) {
      this.cardModel.listId = this.currentListId;
    }
  }

  onSave(){
    if(!this.cardModel.title?.trim()){
      alert("Please enter the card name.")
      return;
    }
    this.savedCard.emit(this.cardModel);
  }

  close(){
    this.cardModel.description = '';

    this.isOpened.emit(false);
  }
}
