import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CardActionEvent, CardItem } from './card-item';
import { DatePipe, NgClass } from '@angular/common';
import { ContextMenu } from "../../context-menu/context-menu";
import { ListItem } from '../list-item';
import { Priority } from './card-item';

@Component({
  selector: 'app-card',
  imports: [DatePipe, ContextMenu, NgClass],
  templateUrl: './card.html',
  styleUrl: './card.css',
})
export class Card {
  @Input({required: true}) cardData!: CardItem;
  @Input({required: true}) lists!: ListItem[];
  @Output() cardAction = new EventEmitter<CardActionEvent>();

  public CardPriorityMap: Record<Priority, string> = {
    [Priority.Low]: 'bg-gray-300',
    [Priority.Medium]: 'bg-gray-500',
    [Priority.High]: 'bg-gray-800',
  }

  onEditCard(){
    this.cardAction.emit({cardAction: 'edit', card: this.cardData});
  }

  onViewCard(){
    this.cardAction.emit({cardAction: 'view', card: this.cardData});
  }

  onDeleteCard(){
    this.cardAction.emit({cardAction: 'delete', card: this.cardData});
  }

  onMoveCard(event: Event){
    const selectedElement = event.target as HTMLSelectElement;
    const targetListId = selectedElement.value;

    this.cardAction.emit({cardAction: 'move', card: this.cardData, desiredListId: targetListId});
    
    selectedElement.value = 'Move to:';
  }
}
