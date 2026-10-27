import { createFeatureSelector, createSelector } from '@ngrx/store';
import { BoardState } from './board.reducer';
import { Priority } from '../models/card.models';

export const selectBoardState = createFeatureSelector<BoardState>('boardFeature');

export const selectBoard = createSelector(selectBoardState, (state) => state.board);

export const selectBoardList = createSelector(selectBoard, (boardDto) => {
  if (!boardDto) return [];

  return boardDto.boardLists.map((list) => ({
    id: list.id,
    boardId: list.boardId,
    title: list.title,
    position: list.position,
    cards: list.cards
      .map((c) => ({
        id: c.id,
        listId: c.boardListId,
        title: c.title,
        description: c.description || '',
        dueDate: c.dueDate || '',
        priority: (c.priority || 'Low') as Priority,
      }))
      .sort((x, y) => {
        if (!x.dueDate) return 1;
        if (!y.dueDate) return -1;
        return new Date(x.dueDate).getTime() - new Date(y.dueDate).getTime();
      }),
  }));
});

export const selectIsLoading = createSelector(selectBoardState, (state) => state.isLoading);
