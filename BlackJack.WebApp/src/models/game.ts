import { Player } from "src/models/player";

export class Game {
    dealer: Player;
    human: Player;
    bots: Player[];
    deck: number[];
    options: string;
}
