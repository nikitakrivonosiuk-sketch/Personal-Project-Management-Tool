import { Component, inject, input, signal } from '@angular/core';
import { HistoryBar } from "./history-bar/history-bar";
import { BoardService } from '../../services/board-service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-header',
  imports: [ HistoryBar, RouterLink],
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

  mainPage(){
    this.boardService.activeBoardId.set(null);
  }
}
