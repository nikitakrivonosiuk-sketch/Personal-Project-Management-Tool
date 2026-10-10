import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from './components/header/header';
import { Board } from "./components/board/board";
import { ToastComponent } from './components/toast.component/toast.component';

@Component({
  selector: 'app-root',
  imports: [Header, RouterOutlet, ToastComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('TaskBoard');
}
