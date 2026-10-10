export enum Priority {
    Low = 'Low',
    Medium = 'Medium',
    High = 'High',
}

export interface CardItem {
    id: string;
    listId: string;
    title: string;
    description?: string;
    dueDate: string;
    priority: Priority;
}

export type CardActionType = 'edit' | 'move' | 'delete' | 'view';

export interface CardActionEvent {
    cardAction: CardActionType;
    card: CardItem;
    desiredListId?: string;
}
