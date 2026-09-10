import { Routes } from '@angular/router';
import { BoardListComponent } from './board-list-component/board-list-component';
import { Board } from './board-list-component/board/board';

export const routes: Routes = [
  { path: 'boards', component: BoardListComponent },
  { path: 'boards/:id', component: Board },
  { path: '', redirectTo: 'boards', pathMatch: 'full' }
];
