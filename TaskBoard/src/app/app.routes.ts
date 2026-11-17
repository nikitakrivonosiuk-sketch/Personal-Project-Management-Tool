import { Routes } from '@angular/router';
import { BoardListComponent } from './components/board-list-component/board-list-component';
import { Board } from './components/board/board';
import { LoginComponent } from './components/login-component/login-component';
import { MainLayoutComponent } from './components/main-layout-component/main-layout-component';
import { RegisterComponent } from './components/register-component/register-component';
import { authGuard } from './services/auth-guard';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },

  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  {
    path: '',
    component: MainLayoutComponent,
    canActivateChild: [authGuard],
    children: [
      { path: 'boards', component: BoardListComponent },
      { path: 'boards/:id', component: Board },
    ],
  },
];