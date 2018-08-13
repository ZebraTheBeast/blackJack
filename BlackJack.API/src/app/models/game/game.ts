import { Player } from "./Player";

export class Game {
    dealer: Player;
    human: Player;
    bots: Player[];
    deck: number[];
    options: string;
}
