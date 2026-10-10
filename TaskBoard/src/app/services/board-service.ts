import { Injectable, inject, signal } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { BoardDto, BoardListDto, CardDto, CreatingBoardRequestDto } from "../models/board-models";
import { Subject } from "rxjs";
import { ActivityLog } from "../models/activity-log.model";
import { environment } from "../../environments/environment";

@Injectable ({
    providedIn: 'root'
})

export class BoardService {
    private http = inject(HttpClient);
    public logsUpdated = new Subject<void>();
    activeBoardId = signal<string | null>(null);

    apiUrl = environment.apiUrl;

    getLogs(filters: {boardId?: string, listId?: string, cardId?: string, skip?: number, take?: number}){
        let params = new HttpParams();

        Object.entries(filters).forEach(([key, value]) => {
            if (value !== undefined && value !== null) {
                params = params.set(key, value.toString());
            }
        });

        return this.http.get<ActivityLog[]>(`${this.apiUrl}/Logs`, {params});
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