import { Component, inject, OnInit, output } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { BoardService } from '../../services/board-service';
import { Store } from '@ngrx/store';
import { BoardActions } from '../../state/board.actions';
import { selectBoard, selectBoardMembers, selectBoardMembersError, selectIsBoardMembersLoading } from '../../state/board.selectors';
import { checkErrors } from '../../services/form-validator';
import { ToastService } from '../../services/toast.service/toast.service';
import { BoardMemberDto, UserLevelAccess } from '../../models/member-models';
import { TokenStorageService } from '../../services/token-storage-service';
import { BoardDto } from '../../models/board-models';

@Component({
  selector: 'app-users-modal',
  imports: [ReactiveFormsModule],
  templateUrl: './users-modal.html',
  styleUrl: './users-modal.css',
})
export class UsersModal implements OnInit {
  boardService = inject(BoardService);
  store = inject(Store);
  toaster = inject(ToastService);
  private tokenStorage = inject(TokenStorageService);

  currentUserId = this.tokenStorage.getUserId();
  board = this.store.selectSignal(selectBoard);

  closed = output<void>();
  activeBoardId = this.boardService.activeBoardId; 

  boardMembers = this.store.selectSignal(selectBoardMembers);
  boardMembersIsLoading = this.store.selectSignal(selectIsBoardMembersLoading);
  boardMembersError = this.store.selectSignal(selectBoardMembersError);

  addUserForm = new FormGroup({
    email: new FormControl('', {nonNullable: true, validators: [Validators.required, Validators.email]}),
    userLevel: new FormControl<UserLevelAccess>(UserLevelAccess.Reader, {nonNullable: true, validators: [Validators.required]}),
  });
  UpdateUserForm = new FormGroup({
    userId: new FormControl('', {nonNullable: true, validators: [Validators.required]}),
    userLevel: new FormControl<UserLevelAccess>(UserLevelAccess.Reader, {nonNullable: true, validators: [Validators.required]}),
  });

  ngOnInit(){
    const boardId = this.activeBoardId();

    if (!boardId) return;

    this.store.dispatch(BoardActions.loadMembers({boardId}))
  }

  canManageMembers(member: BoardMemberDto) : boolean{
    const board = this.board();
    if (!board) return false;
    if (board.isOwner === true) return true;

    return board.currentLevel === UserLevelAccess.Admin && member.userLevel !== UserLevelAccess.Admin;
  }

  closeModal() {
    this.closed.emit();
  }

  onAddMember(){
    const boardId = this.activeBoardId();
    const {email, userLevel} = this.addUserForm.getRawValue();

    const dictionary = {
      email: "Email",
      userLevel: "Role",
    }

    if (!boardId) return;

    const isValid = checkErrors(this.addUserForm, dictionary);

    if (isValid === null){
      this.store.dispatch(BoardActions.addMember({boardId, email, userLevel}));
    }
    else {
      this.addUserForm.markAllAsTouched();
      this.toaster.warning(isValid);
    }
  }

  onUpdateMember(memberUserId: string, event: Event){
    const boardId = this.activeBoardId();
    if (!boardId) return;
    
    const select = event.target as HTMLSelectElement;
    const levelAccess = select.value as UserLevelAccess;
    
    this.store.dispatch(BoardActions.updateMember({boardId, userId: memberUserId, userLevel: levelAccess}));
  }

  onDeleteMember(userId: string){
    const boardId = this.activeBoardId();
    if (!boardId) return;

    this.store.dispatch(BoardActions.deleteMember({userId, boardId}));
  }
}
