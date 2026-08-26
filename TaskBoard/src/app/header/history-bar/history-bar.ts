import { Component, inject, model, OnInit, signal } from '@angular/core';
import { ActivityLog } from '../../board/activity-log';
import { BoardService } from '../../board/board-service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-history-bar',
  imports: [DatePipe],
  templateUrl: './history-bar.html',
  styleUrl: './history-bar.css',
})
export class HistoryBar implements OnInit{
  activityLogs = signal<ActivityLog[]>([]);
  isOpened = model(false);
  httpService = inject(BoardService);

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

    this.httpService.getLogs(undefined, this.skip, this.take).subscribe({
      next: (logs: ActivityLog[]) => {
        if (reset) {
          this.activityLogs.set(logs);
        } else {
          this.activityLogs.update(oldLogs => [...oldLogs, ...logs]);
        }
      },
      error: (err) => console.error("Backend is imposter.", err),
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
