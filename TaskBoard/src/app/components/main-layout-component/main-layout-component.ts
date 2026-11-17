import { Component, inject, signal } from '@angular/core';
import { Header } from '../header/header';
import { Router, RouterOutlet } from '@angular/router';
import { UsersModal } from '../users-modal/users-modal';
import { TokenStorageService } from '../../services/token-storage-service';
import { BoardService } from '../../services/board-service';
import { Store } from '@ngrx/store';
import { BoardActions } from '../../state/board.actions';

@Component({
  selector: 'app-main-layout-component',
  imports: [Header, RouterOutlet, UsersModal],
  templateUrl: './main-layout-component.html',
  styleUrl: './main-layout-component.css',
})
export class MainLayoutComponent {
  tokenStorage = inject(TokenStorageService);
  boardService = inject(BoardService);
  router = inject(Router);
  store = inject(Store);

  membersModalOpen = signal(false);

  onLogout(){
    this.tokenStorage.clearToken();
    this.store.dispatch(BoardActions.logout());
    this.boardService.activeBoardId.set(null);
    this.router.navigateByUrl("/login");
  }
}
