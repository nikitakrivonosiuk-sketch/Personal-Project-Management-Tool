export type TaskStatus = 'todo' | 'planned' | 'in progress' | 'done'; 
export type TaskPriority = 'low' | 'medium' | 'high';

export interface CardItem {
    id: string;
    listId: string;
    title: string;
    description?: string;
    dueDate?: string;
    priority: TaskPriority;
    status: TaskStatus;
}

export type CardActionType = 'edit' | 'move' | 'delete' | 'view';

export interface CardActionEvent {
    cardAction: CardActionType;
    card: CardItem;
}
