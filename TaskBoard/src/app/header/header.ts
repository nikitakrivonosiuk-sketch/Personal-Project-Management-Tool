import { Component, signal } from '@angular/core';
import { Board } from "../board/board";
import { HistoryBar } from "./history-bar/history-bar";

@Component({
  selector: 'app-header',
  imports: [ HistoryBar],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  historyState = signal(false);

  toggle(){
    this.historyState.update(x => !x);
  }
}
