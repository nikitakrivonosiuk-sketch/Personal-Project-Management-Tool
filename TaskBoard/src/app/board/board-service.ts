import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BoardListDto, CardDto } from "./board-model";
import { forkJoin } from "rxjs";

@Injectable ({
    providedIn: 'root'
})

export class BoardService {
    private http = inject(HttpClient);

    apiUrl = "https://localhost:7140/api";

    getBoardData(){
        return forkJoin ({
            lists: this.http.get<BoardListDto[]>(`${this.apiUrl}/Lists`),
            cards: this.http.get<CardDto[]>(`${this.apiUrl}/Cards`)
        });
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
}