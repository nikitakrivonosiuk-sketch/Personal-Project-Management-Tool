import { createReducer, on } from '@ngrx/store';
import { BoardDto } from '../models/board-models';
import { BoardActions } from './board.actions';

export interface BoardState {
  board: BoardDto | null, 
  isLoading: boolean; 
  error: string | null;
}

export const initialState: BoardState = {
    board: null,
    isLoading: false,
    error: null,
}

export const boardReducer = createReducer(
    initialState,

    on(BoardActions.loadBoard, (state) => {
        return {
            ...state,
            isLoading: true,
            error: null,
        }
    }),

    on(BoardActions.loadBoardFailure, (state, {error}) => {
        return {
            ...state,
            error: error,
            isLoading: false,
        }
    }),

    on(BoardActions.loadBoardSuccess, (state, {board}) => {
        return {
            ...state, 
            board: board,
            isLoading: false,
        }
    }),

    // LIST REDUCERS
    on(BoardActions.createListSuccess, (state, {list}) => {
        if (!state.board) return state;

        return {
            ...state, 
            board: {
                ...state.board,
                boardLists: [...state.board.boardLists, list],
            },
        };
    }),
    
    on(BoardActions.renameListSuccess, (state, {list}) => {
        if (!state.board) return state;

        return {
            ...state,
            board: {
                ...state.board,
                boardLists: state.board.boardLists.map(l => l.id === list.id ? 
                    { ...l, title: list.title} : l
                )
            }
        };
    }),

    on(BoardActions.deleteListSuccess, (state, {listId}) => {
        if (!state.board) return state;

        return {
            ...state,
            board: {
                ...state.board,
                boardLists: state.board.boardLists.filter(l => l.id !== listId),
            }
        }
    }),

    // CARDS REDUCERS
    on(BoardActions.createCardSuccess, (state, {card}) => {
        if (!state.board) return state;

        return {
            ...state,
            board: {
                ...state.board,
                boardLists: state.board.boardLists.map(l => l.id === card.boardListId ?
                    {...l, cards: [...l.cards, card]}
                    : l
                )
            }
        }
    }),

    on(BoardActions.editCardSuccess, (state, {card}) => {
        if (!state.board) return state;

        return {
            ...state,
            board: {
                ...state.board,
                boardLists: state.board.boardLists.map(l => {
                    const filteredCards = l.cards.filter(c => c.id !== card.id);

                    if (l.id === card.boardListId){
                        return {
                            ...l,
                            cards: [...filteredCards, card],
                        }
                    }

                    return {
                        ...l,
                        cards: filteredCards,
                    }
                })
            }
        }
    }),

    on(BoardActions.moveCardSuccess, (state, {card}) => {
        if (!state.board) return state;

        return {
            ...state,
            board: {
                ...state.board,
                boardLists: state.board.boardLists.map(l => {
                    const filteredCards = l.cards.filter(c => c.id !== card.id);

                    if (l.id === card.boardListId){
                        return {
                            ...l,
                            cards: [...filteredCards, card],
                        }
                    }

                    return {
                        ...l,
                        cards: filteredCards,
                    }
                })
            }
        }
    }),

    on(BoardActions.deleteCardSuccess, (state, {listId, cardId}) => {
        if (!state.board) return state;

        return {
            ...state,
            board: {
                ...state.board,
                boardLists: state.board.boardLists.map(l => l.id === listId ? {
                    ...l,
                    cards: l.cards.filter(c => c.id !== cardId),
                } : l)
            }
        };
    }),
);