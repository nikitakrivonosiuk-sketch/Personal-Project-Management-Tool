import { Component, inject, OnInit, signal } from '@angular/core';
import { BoardService } from './board/board-service';
import { BoardDto, CreatingBoardRequestDto } from './board/board-model';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ContextMenu } from "./board/context-menu/context-menu";
@Component({
  selector: 'app-board-list-component',
  imports: [RouterLink, FormsModule, ContextMenu],
  templateUrl: './board-list-component.html',
  styleUrl: './board-list-component.css',
})
export class BoardListComponent implements OnInit {
  isAddingBoard: boolean = false;
  editingBoardId: string | null = null;
  newBoardTitle: string = '';
  changedBoardTitle: string = '';


  private boardService = inject(BoardService);
  boards = signal<BoardDto[]>([]);

  ngOnInit(){
    this.boardService.getBoards().subscribe({
      next: (boardsListFromDb : BoardDto[]) => {
        this.boards.set(boardsListFromDb);
      },
      error: (err: any) => console.error("Something wrong with server.", err),
    });
  }

  onCreate(){
    if (this.newBoardTitle === '') {
      alert("You need to enter a title to your board");
      return;
    }

    const boardValue: CreatingBoardRequestDto = {
      title: this.newBoardTitle,
    };

    this.boardService.createBoard(boardValue).subscribe({
      next: (boardFormDb: BoardDto) => {
        this.boards.update(board => [...board, boardFormDb]);
      },
      error: (err: any) => console.error("Something wrong with backend", err),
    });

    this.isAddingBoard = false;
    this.newBoardTitle = '';
  }

  titleChange(board: any){
    this.editingBoardId = board.id; 
  }

  cancelEdit() {
    this.editingBoardId = null;
    this.changedBoardTitle = '';
  }

  onSaveTitle(board: BoardDto){
    if (this.changedBoardTitle === ''){
      alert("Enter the title of your board");
      return;
    }

    const boardToUpdate = {...board, title: this.changedBoardTitle};

    this.boardService.updateBoard(boardToUpdate).subscribe({
      next: (updatedBoardFromDb : BoardDto) => {
        this.boards.update(boards => 
          boards.map(b => b.id === updatedBoardFromDb.id ? updatedBoardFromDb : b));

        this.editingBoardId = null;
      },
      error: (err : any) => console.error("Something wrong with backend.", err),
    })
  }

  boardDelete(id: string){
    this.boardService.deleteBoard(id).subscribe({
      next: () => {
        this.boards.update(board => board.filter(b => b.id !== id));
      },
      error: (err: any) => console.error("Something wrong with back."),
    });
  }

  onAddBoardClick(){
    this.isAddingBoard = true;
  }

  close(){
    this.isAddingBoard = false;
    this.newBoardTitle = '';
  }
}
