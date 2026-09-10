import { CardItem } from "./card/card-item";

export interface ListItem {
    id: string;
    boardId: string;
    title: string;
    position: number;
    cards: CardItem[];
}