import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CardItem } from '../list/card/card-item';
import { DatePipe } from '@angular/common';
import { ListItem } from '../list/list-item';

@Component({
  selector: 'app-card-view-modal',
  imports: [DatePipe],
  templateUrl: './card-view-modal.html',
  styleUrl: './card-view-modal.css',
})
export class CardViewModal {
  @Output() isOpened = new EventEmitter<boolean>();
  @Output() isEditing = new EventEmitter<CardItem>();
  @Input({required: true}) cardInfo!: CardItem;
  @Input({required: true}) lists!: ListItem[]; 
  cardList: string = '';

  ngOnInit(){
    this.cardList = this.lists.find(x => x.id === this.cardInfo.listId)?.title || 'Unknown List';
  }

  onClose(){
    this.isOpened.emit(false);
  }

  onEditClick(){
    this.isOpened.emit(false);
    this.isEditing.emit(this.cardInfo);
  }
}
