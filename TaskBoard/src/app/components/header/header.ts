import { Component, inject, input, output, signal } from '@angular/core';
import { HistoryBar } from "./history-bar/history-bar";
import { BoardService } from '../../services/board-service';
import { RouterLink } from '@angular/router';
import { TokenStorageService } from '../../services/token-storage-service';

@Component({
  selector: 'app-header',
  imports: [ HistoryBar, RouterLink],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  boardService = inject(BoardService);
  tokenStorage = inject(TokenStorageService);

  addMembers = output<void>();
  logout = output<void>();
  
  activeBoardId = this.boardService.activeBoardId; 
  historyState = signal(false);

  logoutShow = signal(!!this.tokenStorage.getToken());

  toggle(){
    this.historyState.update(x => !x);
  }

  mainPage(){
    this.boardService.activeBoardId.set(null);
  }
}
