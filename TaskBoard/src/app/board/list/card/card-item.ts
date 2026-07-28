export type TaskStatus = 'todo' | 'planned' | 'in progress' | 'done'; 
export type TaskPriority = 'low' | 'medium' | 'high';

export interface CardItem {
    id: string;
    title: string;
    description?: string;
    dueDate?: string;
    priority: TaskPriority;
    status: TaskStatus;
}
