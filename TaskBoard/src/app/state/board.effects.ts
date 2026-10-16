import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { BoardService } from "../services/board-service";
import { BoardActions } from "./board.actions";
import { catchError, map, of, switchMap } from "rxjs";
import { ToastService } from "../services/toast.service/toast.service";
import { BoardListDto, CardDto } from "../models/board-models";

@Injectable()
export class BoardEffects {
    private actions$ = inject(Actions);
    private boardService = inject(BoardService);
    private toaster = inject(ToastService);

    fetchBoard$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(BoardActions.loadBoard),

            switchMap((action) => {
                return this.boardService.getBoardById(action.boardId).pipe(
                    map((boardData) => {
                        this.boardService.activeBoardId.set(action.boardId);
                        return BoardActions.loadBoardSuccess({board: boardData})
                    }),

                    catchError((error) => {
                        this.toaster.error(error.message);
                        return of(BoardActions.loadBoardFailure({error: error.message}));
                    }),
                )
            }
        ))
    });

    // LIST EFFECTS
    createList$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(BoardActions.createList),

            switchMap((action) => {
                return this.boardService.createList(action.list).pipe(
                    map((createdList: BoardListDto) => {
                        this.boardService.logsUpdated.next();
                        return BoardActions.createListSuccess({list: createdList});
                    })
                )
            })
        )
    });

    renameList$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(BoardActions.renameList),

            switchMap((action) => {
                const boardListDto = {
                    id: action.list.id,
                    title: action.list.title,
                    position: action.list.position,
                }

                return this.boardService.updateList(boardListDto.id, boardListDto).pipe(
                    map(() => {
                        this.boardService.logsUpdated.next();
                        return BoardActions.renameListSuccess({list: action.list});
                    }),

                    catchError((error) => {
                        this.toaster.error("Failed to rename list.");
                        return of(BoardActions.renameListFailure({error: error.message}));
                    })
                );
            }),
        );
    });

    deleteList$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(BoardActions.deleteList),

            switchMap((action) => {
                return this.boardService.deleteList(action.listId).pipe(
                    map(() => {
                        this.boardService.logsUpdated.next();
                        return BoardActions.deleteListSuccess({listId: action.listId});
                    }),

                    catchError((error) => {
                        this.toaster.error("Failed to delete list");
                        return of(BoardActions.deleteCardFailure({error: error.message}));
                    })
                )
            })
        )
    });

    // CARDS EFFECTS
    createCard$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(BoardActions.createCard),

            switchMap((action) => {
                return this.boardService.createCard(action.card).pipe(
                    map((card: CardDto) => {
                        this.boardService.logsUpdated.next();
                        return BoardActions.createCardSuccess({card: card});
                    }),

                    catchError((error) => {
                        this.toaster.error("Failed to create card.");
                        return of(BoardActions.createCardFailure({error: error.message}));
                    }),
                );
            })
        )
    });

    editCard$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(BoardActions.editCard),

            switchMap((action) => {
                const cardDto = {
                    id: action.card.id,
                    title: action.card.title,
                    dueDate: action.card.dueDate,
                    description: action.card.description,
                    boardListId: action.card.listId,
                    priority: action.card.priority,
                }

                return this.boardService.updateCard(cardDto.id, cardDto).pipe(
                    map((cardDto: CardDto) => {
                        this.boardService.logsUpdated.next();
                        return BoardActions.editCardSuccess({card: cardDto})
                    }),

                    catchError((error) => {
                        this.toaster.error("Failed to update card.");
                        return of(BoardActions.editCardFailure({error: error.message}))
                    })
                )
            })
        )
    });

    moveCard$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(BoardActions.moveCard),

            switchMap((action) => {
                const cardDto = {
                    id: action.card.id,
                    title: action.card.title,
                    dueDate: action.card.dueDate,
                    description: action.card.description,
                    boardListId: action.card.listId,
                    priority: action.card.priority,
                }

                return this.boardService.updateCard(cardDto.id, cardDto).pipe(
                    map((card: CardDto) => {
                    this.boardService.logsUpdated.next();
                    return BoardActions.moveCardSuccess({card: card});
                }),

                catchError((error) => {
                    this.toaster.error("Failed to move card.");
                    return of(BoardActions.moveCardFailure({error: error.message}));
                }))
            }),
        )
    });

    deleteCard$ = createEffect(() => {
        return this.actions$.pipe(
            ofType(BoardActions.deleteCard),

            switchMap((action) => {
                return this.boardService.deleteCard(action.cardId).pipe(
                    map(() => {
                        this.boardService.logsUpdated.next();
                        return BoardActions.deleteCardSuccess({listId: action.listId, cardId: action.cardId});
                    }),

                    catchError((error) => {
                        this.toaster.error("Failed to delete.");
                        return of(BoardActions.deleteCardFailure({error: error.message}));
                    })
                )
            })
        )
    })
}