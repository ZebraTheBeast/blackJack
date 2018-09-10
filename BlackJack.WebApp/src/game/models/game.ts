import { Player } from "src/game/models/player";

export class Game {
    dealer: Player;
    human: Player;
    bots: Player[];
    deck: number[];
    options: string;
}
