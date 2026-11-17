import { Component, inject, input, model, OnInit, signal } from '@angular/core';
import { ActivityLog } from '../../../models/activity-log.model';
import { BoardService } from '../../../services/board-service';
import { DatePipe } from '@angular/common';
import { ToastService } from '../../../services/toast.service/toast.service';

@Component({
  selector: 'app-history-bar',
  imports: [DatePipe],
  templateUrl: './history-bar.html',
  styleUrl: './history-bar.css',
})
export class HistoryBar implements OnInit{
  boardId = input.required<string>();
  activityLogs = signal<ActivityLog[]>([]);
  isOpened = model(false);
  httpService = inject(BoardService);
  toaster = inject(ToastService);

  skip = 0;
  readonly take = 10;

  ngOnInit() {
    this.loadLogs(true);

    this.httpService.logsUpdated.subscribe(() => {
      this.loadLogs(true);
    });
  }

  loadLogs(reset: boolean = false) {
    if (reset) {
      this.skip = 0;
    }

    this.httpService.getLogs({boardId:this.boardId(), skip: this.skip, take: this.take}).subscribe(
    {
      next: (logs: ActivityLog[]) => {
        if (reset) {
          this.activityLogs.set(logs);
        } else {
          this.activityLogs.update(oldLogs => [...oldLogs, ...logs]);
        }
      },
      error: (err) => this.toaster.error("Server isn't working properly."),
    });
  }

  loadMore() {
    this.skip += this.take;
    this.loadLogs(false);
  }

  close(){
    this.isOpened.set(false);
  }
}
