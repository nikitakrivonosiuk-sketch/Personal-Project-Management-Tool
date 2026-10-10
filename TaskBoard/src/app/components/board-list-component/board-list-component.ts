import { Component, inject, OnInit, signal } from '@angular/core';
import { BoardService } from '../../services/board-service';
import { BoardDto, CreatingBoardRequestDto } from '../../models/board-models';
import { RouterLink } from '@angular/router';
import { FormControl, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ContextMenu } from '../context-menu/context-menu';
import { ToastService } from '../../services/toast.service/toast.service';
import { checkErrors } from '../../services/form-validator';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-board-list-component',
  imports: [RouterLink, FormsModule, ContextMenu, ReactiveFormsModule],
  templateUrl: './board-list-component.html',
  styleUrl: './board-list-component.css',
})
export class BoardListComponent implements OnInit {
  isAddingBoard: boolean = false;
  editingBoardId: string | null = null;

  private toaster = inject(ToastService);

  dictionary = {
    newTitle: 'Board title',
    changedBoardTitle: 'Board title',
  };

  createForm = new FormGroup({
    newTitle: new FormControl('', [Validators.required, Validators.maxLength(20)]),
  });

  editForm = new FormGroup({
    changedTitle: new FormControl('', [Validators.required, Validators.maxLength(20)]),
  });

  private boardService = inject(BoardService);
  boards = signal<BoardDto[]>([]);

  ngOnInit() {
    this.boardService.getBoards().subscribe({
      next: (boardsListFromDb: BoardDto[]) => {
        this.boards.set(boardsListFromDb);
      },
      error: (err: any) => this.toaster.error('Something went wrong with backend.'),
    });
  }

  onCreate() {
    const isValid = checkErrors(this.createForm, this.dictionary);
    if (isValid !== null) {
      this.toaster.warning(isValid);
      return;
    }

    const boardValue: CreatingBoardRequestDto = {
      title: this.createForm.value.newTitle as string,
    };

    this.boardService.createBoard(boardValue).subscribe({
      next: (boardFormDb: BoardDto) => {
        this.boards.update((board) => [...board, boardFormDb]);
      },
      error: (err: any) => this.toaster.error('Something went wrong with backend.'),
    });

    this.isAddingBoard = false;
    this.createForm.patchValue({ newTitle: '' });
  }

  titleChange(board: any) {
    this.editingBoardId = board.id;
  }

  cancelEdit() {
    this.editingBoardId = null;
    this.editForm.patchValue({ changedTitle: '' });
  }

  onSaveTitle(board: BoardDto) {
    const isValid = checkErrors(this.editForm, this.dictionary);
    if (isValid !== null) {
      this.toaster.warning(isValid);
      return;
    }

    const boardToUpdate = { ...board, title: this.editForm.value.changedTitle as string};

    this.boardService.updateBoard(boardToUpdate).subscribe({
      next: (updatedBoardFromDb: BoardDto) => {
        this.boards.update((boards) =>
          boards.map((b) => (b.id === updatedBoardFromDb.id ? updatedBoardFromDb : b)),
        );

        this.editingBoardId = null;
      },
      error: (err: any) => this.toaster.error('Something went wrong with backend.'),
    });
  }

  boardDelete(id: string) {
    this.boardService.deleteBoard(id).subscribe({
      next: () => {
        this.boards.update((board) => board.filter((b) => b.id !== id));
      },
      error: (err: any) => this.toaster.error('Something went wrong with backend.'),
    });
  }

  onAddBoardClick() {
    this.isAddingBoard = true;
  }

  close() {
    this.isAddingBoard = false;
    this.createForm.patchValue({ newTitle: '' });
  }
}
