import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from "./header/header";
import { Board } from "./board-list-component/board/board";
import { ToastComponent } from './toast.service/toast.component/toast.component';

@Component({
  selector: 'app-root',
  imports: [Header, RouterOutlet, ToastComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('TaskBoard');
}
