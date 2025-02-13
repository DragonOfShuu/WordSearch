export type WordDictionary = {
    [word: string]: WordType
}

export type WordType = {
    Word: string,
    Position: [number, number]
    Rotation: [number, number]
}
