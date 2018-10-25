﻿import { DealerViewItem } from "src/app/shared/models/dealer-view-item.model";
import { PlayerViewItem } from "src/app/shared/models/player-view-item.model";

export class ResponseLoadMatchGameView {
    gameId: number;
    dealer: DealerViewItem;
    human: PlayerViewItem;
    bots: PlayerViewItem[];
    deck: number[];
    options: string;
}