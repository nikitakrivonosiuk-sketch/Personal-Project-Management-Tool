import { Component, model } from '@angular/core';

@Component({
  selector: 'app-history-bar',
  imports: [],
  templateUrl: './history-bar.html',
  styleUrl: './history-bar.css',
})
export class HistoryBar {
  isOpened = model(false);

  close(){
    this.isOpened.set(false);
  }
}
