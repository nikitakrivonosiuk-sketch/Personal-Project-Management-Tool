import { Component, EventEmitter, input, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Card } from './card/card';
import { ListItem } from './list-item';
import { ContextMenu } from "../context-menu/context-menu";
import { CardActionEvent, CardActionType } from './card/card-item';


@Component({
  selector: 'app-list',
  imports: [Card, ContextMenu, FormsModule],
  templateUrl: './list.html',
  styleUrl: './list.css',
})
export class List {
  @Input({required: true}) lists: ListItem[] = [];
  @Input({required: true}) listData!: ListItem;
  @Output() deleteList = new EventEmitter<void>();
  @Output() addCard = new EventEmitter<string>();
  @Output() cardAction = new EventEmitter<CardActionEvent>();
  @Output() updateList = new EventEmitter<ListItem>();


  isEditing = false;
  newName: string = '';

  onEditListClick(){
    this.newName = this.listData.title;
    this.isEditing = true;
  }

  onAddCardClick(){
    this.addCard.emit(this.listData.id);
  }

  Save(){
    this.listData.title = this.newName;

    // Saving to db.
    this.updateList.emit(this.listData);

    this.isEditing = false;
  }

  DeleteList(){
    this.deleteList.emit();
  }

  // Card handling
  onHandleCard(event: CardActionEvent){
    this.cardAction.emit(event);
  }
}
