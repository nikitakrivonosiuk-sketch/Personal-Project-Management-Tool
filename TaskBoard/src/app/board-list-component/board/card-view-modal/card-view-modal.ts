import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { CardItem } from '../list/card/card-item';
import { DatePipe } from '@angular/common';
import { ListItem } from '../list/list-item';
import { ActivityLog } from '../activity-log';
import { BoardService } from '../board-service';

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

    this.httpService.getLogs(this.cardInfo.id, this.skip, this.take).subscribe({
      next: (loadedLogs: ActivityLog[]) => {
        if(reset){
          this.cardLogs.set(loadedLogs);
        }
        else {
          this.cardLogs.update(oldLogs => [...oldLogs, ...loadedLogs]);
        }
      },
      error: (err) => console.error("Backend is imposter", err),
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
