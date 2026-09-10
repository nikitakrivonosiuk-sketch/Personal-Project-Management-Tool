export type TaskPriority = 'Low' | 'Medium' | 'High';

export interface CardItem {
    id: string;
    listId: string;
    title: string;
    description?: string;
    dueDate?: string;
    priority: TaskPriority;
}

export type CardActionType = 'edit' | 'move' | 'delete' | 'view';

export interface CardActionEvent {
    cardAction: CardActionType;
    card: CardItem;
    desiredListId?: string;
}
