import { Component, inject, input, signal } from '@angular/core';
import { HistoryBar } from "./history-bar/history-bar";
import { BoardService } from '../board-list-component/board/board-service';

@Component({
  selector: 'app-header',
  imports: [ HistoryBar],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  boardService = inject(BoardService);
  
  activeBoardId = this.boardService.activeBoardId; 
  historyState = signal(false);

  toggle(){
    this.historyState.update(x => !x);
  }
}
