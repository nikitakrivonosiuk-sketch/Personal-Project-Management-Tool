import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { CardItem } from '../../models/card.models';
import { DatePipe } from '@angular/common';
import { ListItem } from '../../models/list-model';
import { ActivityLog } from '../../models/activity-log.model';
import { BoardService } from '../../services/board-service';
import { ToastService } from '../../services/toast.service/toast.service';

@Component({
  selector: 'app-card-view-modal',
  imports: [DatePipe],
  templateUrl: './card-view-modal.html',
  styleUrl: './card-view-modal.css',
})
export class CardViewModal implements OnInit {
  @Output() isOpened = new EventEmitter<boolean>();
  @Output() isEditing = new EventEmitter<CardItem>();
  @Input({required: true}) cardInfo!: CardItem;
  @Input({required: true}) lists!: ListItem[]; 
  cardList: string = '';

  toaster = inject(ToastService);

  cardLogs = signal<ActivityLog[]>([]);
  skip: number = 0;
  take: number = 10;
  httpService = inject(BoardService);

  ngOnInit(){
    this.cardList = this.lists.find(x => x.id === this.cardInfo.listId)?.title || 'Unknown List';

    this.loadCardLogs(true);
    
    this.httpService.logsUpdated.subscribe(() => {
      this.loadCardLogs(true);
    });
  }

  loadCardLogs(reset: boolean = false) {
    if (reset) {
      this.skip = 0;
    }

    this.httpService.getLogs({cardId: this.cardInfo.id, skip: this.skip, take: this.take}).subscribe({
      next: (loadedLogs: ActivityLog[]) => {
        if(reset){
          this.cardLogs.set(loadedLogs);
        }
        else {
          this.cardLogs.update(oldLogs => [...oldLogs, ...loadedLogs]);
        }
      },
      error: (err) => this.toaster.error('Something went wrong with backend.'),
    });
  }
  
  loadMoreLogs() {
    this.skip += this.take;
    this.loadCardLogs(false);
  }

  onClose(){
    this.isOpened.emit(false);
  }

  onEditClick(){
    this.isOpened.emit(false);
    this.isEditing.emit(this.cardInfo);
  }
}
