import { Component, ElementRef, HostListener, Input } from '@angular/core';
import { Card } from './card/card';
import { TaskList } from './list-item';
import { ContextMenu } from "../context-menu/context-menu";

@Component({
  selector: 'app-list',
  imports: [Card, ContextMenu],
  templateUrl: './list.html',
  styleUrl: './list.css',
})
export class List {
  @Input({required: true}) listData!: TaskList;
}
