import { CardItem } from "./card.models";

export interface ListItem {
    id: string;
    boardId: string;
    title: string;
    position: number;
    cards: CardItem[];
}

