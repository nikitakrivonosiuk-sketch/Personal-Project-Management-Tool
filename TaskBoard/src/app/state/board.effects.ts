import { inject, Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { BoardService } from "../services/board-service";
import { BoardActions } from "./board.actions";
import { catchError, map, of, switchMap } from "rxjs";
import { ToastService } from "../services/toast.service/toast.service";
import { BoardListDto, CardDto } from "../models/board-models";
import { HttpErrorResponse } from "@angular/common/http";
import { ApiErrorHandlerService } from "../services/api-error-handler-service";

@Injectable()
export class BoardEffects {
  private actions$ = inject(Actions);
  private boardService = inject(BoardService);
  private toaster = inject(ToastService);
  private apiErrorService = inject(ApiErrorHandlerService);

  fetchBoardMembers$ = createEffect(() => {
    return this.actions$.pipe(
        ofType(BoardActions.loadMembers),

        switchMap((action) => {
            return this.boardService.getBoardMembers(action.boardId).pipe(
                map((boardMembersFromDb) => {
                    return BoardActions.loadMembersSuccess( {boardMembers: boardMembersFromDb} )
                }),

                catchError((error : HttpErrorResponse) => {
                    const message = this.apiErrorService.handle(error, 'Unable to load members.');
                    return of(BoardActions.loadMembersFailure({error: message}));
                })
            )
        })
    )
  });

  addBoardMember$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(BoardActions.addMember),

      switchMap((action) => {
        return this.boardService.addBoardMember(action.boardId, action.email, action.userLevel).pipe(
          map((boarbMemberFromDb) => {
            return BoardActions.addMemberSuccess({boardMember: boarbMemberFromDb})
          }),

          catchError((error : HttpErrorResponse) => {
            const message = this.apiErrorService.handle(error, 'Unable to add member.');
            return of(BoardActions.addMemberFailure({error: message}));
          })
        )
      })
    )
  });

  updateBoardMember$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(BoardActions.updateMember),

      switchMap((action) => {
        return this.boardService.updateBoardMember(action.boardId, action.userLevel, action.userId).pipe(
          map((updatedBoardMember) => {
            return BoardActions.updateMemberSuccess({boardMember: updatedBoardMember});
          }),

          catchError((error) => {
            const message = this.apiErrorService.handle(error, 'Unable to update member`s role.');
            return of(BoardActions.updateMemberFailure({error: message}));
          })
        )
      })
    )
  });

  deleteBoardMember$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(BoardActions.deleteMember),

      switchMap((action) => {
        return this.boardService.deleteBoardMember(action.userId, action.boardId).pipe(
          map((result: string) => {
            this.toaster.success(result);
            return BoardActions.deleteMemberSuccess({userId: action.userId});
          }),

          catchError((error : HttpErrorResponse) => {
            const message = this.apiErrorService.handle(error, 'Unable to delete member.');
            return of(BoardActions.deleteMemberFailure({error: message}));
          }),
        )
      })
    )
  });
  
  // Board Effevts  
  fetchBoard$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(BoardActions.loadBoard),

      switchMap((action) => {
        return this.boardService.getBoardById(action.boardId).pipe(
          map((boardData) => {
            this.boardService.activeBoardId.set(action.boardId);
            return BoardActions.loadBoardSuccess({ board: boardData });
          }),

          catchError((error : HttpErrorResponse) => {
            const message = this.apiErrorService.handle(error, 'Unable to load boards.');
            return of(BoardActions.loadBoardFailure({ error: message }));
          }),
        );
      }),
    );
  });

  // LIST EFFECTS
  createList$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(BoardActions.createList),

      switchMap((action) => {
        return this.boardService.createList(action.list).pipe(
          map((createdList: BoardListDto) => {
            this.boardService.logsUpdated.next();
            return BoardActions.createListSuccess({ list: createdList });
          }),

          catchError((error) => {
            const message = this.apiErrorService.handle(error, 'Unable to create list.');
            return of(BoardActions.createListFailure({error: message}));
          }),
        );
      }),
    );
  });

  renameList$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(BoardActions.renameList),

      switchMap((action) => {
        const boardListDto = {
          id: action.list.id,
          title: action.list.title,
          position: action.list.position,
        };

        return this.boardService.updateList(boardListDto.id, boardListDto).pipe(
          map(() => {
            this.boardService.logsUpdated.next();
            return BoardActions.renameListSuccess({ list: action.list });
          }),

          catchError((error) => {
            const message = this.apiErrorService.handle(error, 'Unable to rename list.');
            return of(BoardActions.renameListFailure({ error: message }));
          }),
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
            return BoardActions.deleteListSuccess({ listId: action.listId });
          }),

          catchError((error) => {
            const message = this.apiErrorService.handle(error, 'Unable to delete list.');
            return of(BoardActions.deleteListFailure({ error: message }));
          }),
        );
      }),
    );
  });

  // CARDS EFFECTS
  createCard$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(BoardActions.createCard),

      switchMap((action) => {
        return this.boardService.createCard(action.card).pipe(
          map((card: CardDto) => {
            this.boardService.logsUpdated.next();
            return BoardActions.createCardSuccess({ card: card });
          }),

          catchError((error) => {
            const message = this.apiErrorService.handle(error, 'Unable to create card.');
            return of(BoardActions.createCardFailure({ error: message }));
          }),
        );
      }),
    );
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
        };

        return this.boardService.updateCard(cardDto.id, cardDto).pipe(
          map((cardDto: CardDto) => {
            this.boardService.logsUpdated.next();
            return BoardActions.editCardSuccess({ card: cardDto });
          }),

          catchError((error) => {
            const message = this.apiErrorService.handle(error, 'Unable to update card.');
            return of(BoardActions.editCardFailure({ error: message }));
          }),
        );
      }),
    );
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
        };

        return this.boardService.updateCard(cardDto.id, cardDto).pipe(
          map((card: CardDto) => {
            this.boardService.logsUpdated.next();
            return BoardActions.moveCardSuccess({ card: card });
          }),

          catchError((error) => {
            const message = this.apiErrorService.handle(error, 'Unable to move card.');
            return of(BoardActions.moveCardFailure({ error: message }));
          }),
        );
      }),
    );
  });

  deleteCard$ = createEffect(() => {
    return this.actions$.pipe(
      ofType(BoardActions.deleteCard),

      switchMap((action) => {
        return this.boardService.deleteCard(action.cardId).pipe(
          map(() => {
            this.boardService.logsUpdated.next();
            return BoardActions.deleteCardSuccess({ listId: action.listId, cardId: action.cardId });
          }),

          catchError((error) => {
            const message = this.apiErrorService.handle(error, 'Failed to delete card.');
            return of(BoardActions.deleteCardFailure({ error: message }));
          }),
        );
      }),
    );
  });
}