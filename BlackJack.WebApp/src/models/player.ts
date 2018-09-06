import { Hand } from "src/models/hand";

export class Player {
    id: number;
    name: string;
    points: number;
    hand: Hand;
    betValue: number;
}
