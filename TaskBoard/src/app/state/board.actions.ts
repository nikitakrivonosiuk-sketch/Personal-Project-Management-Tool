import { createActionGroup, props } from '@ngrx/store';
import { BoardDto, BoardListDto, CardDto, CreateCardRequestDto, CreateListRequestDto } from '../models/board-models';
import { ListItem } from '../models/list-model';
import { CardItem } from '../models/card.models';

export const BoardActions = createActionGroup({
  source: 'Board Page',
  events: {
    'Load Board': props<{ boardId: string }>(),
    'Load Board Success': props<{board: BoardDto}>(),
    'Load Board Failure': props<{error: string}>(),

    // Cards Actions
    'Create Card': props<{ card: CreateCardRequestDto }>(),
    'Create Card Success': props<{card: CardDto}>(),
    'Create Card Failure': props<{error: string}>(),

    'Edit Card': props<{ card: CardItem }>(),
    'Edit Card Success': props<{card: CardDto}>(),
    'Edit Card Failure': props<{error: string}>(),

    'Move Card': props<{ card: CardItem }>(),
    'Move Card Success': props<{card: CardDto}>(),
    'Move Card Failure': props<{error: string}>(),

    'Delete Card': props<{listId: string; cardId: string}>(),
    'Delete Card Success': props<{listId: string, cardId: string}>(),
    'Delete Card Failure': props<{error: string}>(),

    // List Actions
    'Create List': props<{list: CreateListRequestDto}>(),
    'Create List Success': props<{list: BoardListDto}>(),
    'Create List Failure': props<{error: string}>(),

    'Rename List': props<{list: ListItem}>(),
    'Rename List Success': props<{list: ListItem}>(),
    'Rename List Failure': props<{error: string}>(),

    'Delete List': props<{listId: string}>(),
    'Delete List Success': props<{listId: string}>(),
    'Delete List Failure': props<{error: string}>(),
  },
});
