import { CardItem } from "./card/card-item";

export interface TaskList {
    id: string;
    title: string;
    cards: CardItem[];
}