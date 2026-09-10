import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BoardDto, BoardListDto, CardDto, CreatingBoardRequestDto } from "./board-model";
import { forkJoin, Subject } from "rxjs";
import { ActivityLog } from "./activity-log";

@Injectable ({
    providedIn: 'root'
})

export class BoardService {
    private http = inject(HttpClient);
    public logsUpdated = new Subject<void>();

    apiUrl = "https://localhost:7140/api";

    getLogs(cardId?: string, skip: number = 0, take: number = 10){
        let url = `${this.apiUrl}/Logs?skip=${skip}&take=${take}`;

        if (cardId){
            url += `&cardId=${cardId}`
        }
        return this.http.get<ActivityLog[]>(url);
    }

    getBoardData(boardId: string){
        return this.http.get<BoardDto>(`${this.apiUrl}/Boards/${boardId}`);
    }

    // Card functions
    createCard(cardData: any){
        return this.http.post<CardDto>(`${this.apiUrl}/Cards`, cardData);
    }

    updateCard(id: string, cardData: any){
        return this.http.put<CardDto>(`${this.apiUrl}/Cards/${id}`, cardData);
    }

    deleteCard(id: string){
        return this.http.delete(`${this.apiUrl}/Cards/${id}`, {responseType: 'text'});
    }

    // List functions
    createList(boardList: any){
        return this.http.post<BoardListDto>(`${this.apiUrl}/Lists`, boardList);
    }

    updateList(listId: string ,boardList: any){
        return this.http.put<BoardListDto>(`${this.apiUrl}/Lists/${listId}`, boardList);
    }

    deleteList(listId: string){
        return this.http.delete(`${this.apiUrl}/Lists/${listId}`, {responseType: 'text'});
    }

    // Board actions
    getBoards(){
        return this.http.get<BoardDto[]>(`${this.apiUrl}/Boards`);
    }

    getBoardById(boardId: string) {
        return this.http.get<BoardDto>(`${this.apiUrl}/Boards/${boardId}`);
    }

    createBoard(boardDto: CreatingBoardRequestDto){
        return this.http.post<BoardDto>(`${this.apiUrl}/Boards`, boardDto);
    }

    updateBoard(board: BoardDto){
        return this.http.put<BoardDto>(`${this.apiUrl}/Boards/${board.id}`, board);
    }

    deleteBoard(id: string){
        return this.http.delete(`${this.apiUrl}/Boards/${id}`, {responseType: 'text'});
    }
}