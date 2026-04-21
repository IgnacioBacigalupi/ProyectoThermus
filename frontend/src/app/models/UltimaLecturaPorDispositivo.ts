export interface UltimaLecturaPorDispositivo {
    
    deviceId: number;

    externalId: string;

    name?: string | null;

    location?: string | null;

    temperature: number;

    humidity: number;

    takenAtUtc: string;

}

