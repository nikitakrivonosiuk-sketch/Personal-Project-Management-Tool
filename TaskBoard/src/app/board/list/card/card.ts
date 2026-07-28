import { Component, Input } from '@angular/core';
import { CardItem } from './card-item';
import { DatePipe } from '@angular/common';
import { ContextMenu } from "../../context-menu/context-menu";

@Component({
  selector: 'app-card',
  imports: [DatePipe, ContextMenu],
  templateUrl: './card.html',
  styleUrl: './card.css',
})
export class Card {
  @Input({required: true}) cardData!: CardItem;
}
