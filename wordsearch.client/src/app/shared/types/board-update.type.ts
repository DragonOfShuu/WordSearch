import { Board } from "./boards.types"

export type BoardUpdateType = {
    board: Board
    newBoard?: Board
    xpGain: number
    wordsFound: string[]
}
