import { createReducer, on } from '@ngrx/store';
import { BoardDto } from '../models/board-models';
import { BoardActions } from './board.actions';
import { BoardMemberDto } from '../models/member-models';

export interface BoardState {
  board: BoardDto | null, 
  isLoading: boolean; 
  error: string | null;
}

export interface BoardMembersState {
    boardMembers: BoardMemberDto[] | null,
    membersLoading: boolean,
    error: string | null,  
}

export const initialState: BoardState = {
    board: null,
    isLoading: false,
    error: null,
}

export const initialBoardMembersState: BoardMembersState = {
    boardMembers: [],
    membersLoading: false,
    error: null,
}

export const boardReducer = createReducer(
    initialState,

    on(BoardActions.logout, () => {
        return initialState;
    }),

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

export const boardMembersReducer = createReducer(
    initialBoardMembersState,

    on(BoardActions.logout, () => {
        return initialBoardMembersState;
    }),

    on(BoardActions.loadMembers, (state) => {
        return {...state, 
            membersLoading: true,
            error: null};
    }),

    on(BoardActions.loadMembersSuccess, (state, {boardMembers}) => {
        return {
            ...state, 
            boardMembers: boardMembers,
            membersLoading: false,
            error: null
        }
    }),

    on (BoardActions.loadMembersFailure, (state, {error}) => {
        return {
            ...state,
            error: error,
            membersLoading: false,
        }
    }),

    on(BoardActions.addMember, (state) => {
        return {
            ...state, 
            error: null,
        };
    }),

    on(BoardActions.addMemberSuccess, (state, {boardMember}) => ({
            boardMembers: [...(state.boardMembers ?? []), boardMember],
            membersLoading: false,
            error: null,
    })),

    on(BoardActions.addMemberFailure, (state, {error}) => ({
            ...state,
            error: error,
            membersLoading: false,
    })),

    // Update board member
    on(BoardActions.updateMember, (state) => {
        return {
            ...state, 
            error: null,
        };
    }),

    on(BoardActions.updateMemberSuccess, (state, {boardMember}) => {
        return {
            ...state,
            boardMembers: (state.boardMembers ?? []).map(member => 
                member.userId === boardMember.userId ? boardMember : member
            ),
            membersLoading: false,
            error: null,
        }
    }),

    on(BoardActions.updateMemberFailure, (state, {error}) => ({
        ...state,
        error: error,
        membersLoading: false,
    })),

    // Delete board member
    on(BoardActions.deleteMember, (state) => {
        return {
            ...state,
            error: null,
        };
    }),

    on(BoardActions.deleteMemberSuccess, (state, {userId}) => {
        return {
            ...state,
            boardMembers: (state.boardMembers ?? []).filter(member => member.userId !== userId),
            membersLoading: false,
            error: null
        }
    }),

    on(BoardActions.deleteMemberFailure, (state, {error}) => ({
        ...state,
        error: error,
        membersLoading: false,
    })),
);