import { useEffect, useState } from "react";
import { deleteMand, deleteOldestFruit, getMandDetail  } from "../services/api";
import { FruitItem, MandDetail,  } from "../types/Fruit";

interface Props {
  setRefreshFunction: (fn: () => void) => void;
}

export default function BasketOverview({ setRefreshFunction }: Props) {
  const [loading, setLoading] = useState(true);
  const [mandDetail, setMandDetail] = useState<MandDetail | null>(null);
  const [oldestFruits, setOldestFruits] = useState<Record<string, FruitItem>>({});

  useEffect(() => {
    setRefreshFunction(refreshBasket);
    refreshBasket();
  }, []);

  const refreshBasket = async () => {
    setLoading(true);
    try {
      const detail = await getMandDetail();
      setMandDetail(detail);
  
      const oldest: Record<string, FruitItem> = {};
  
      for (const fruit of detail.fruits) {
        const soort = fruit.soortFruit;
        if (!oldest[soort] || fruit.aankoopDatum < oldest[soort].aankoopDatum) {
          oldest[soort] = fruit;
        }
      }
  
      setOldestFruits(oldest);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteOldestOverall = async () => {
    if (!mandDetail || mandDetail.fruits.length === 0) {
      alert("Geen fruit om te verwijderen.");
      return;
    }
  
    const sorted = [...mandDetail.fruits].sort(
      (a, b) => new Date(a.aankoopDatum).getTime() - new Date(b.aankoopDatum).getTime()
    );
  
    const oldest = sorted[0];
    if (!oldest) {
      alert("Kon het oudste fruit niet bepalen.");
      return;
    }
  
    await deleteOldestFruit(oldest.soortFruit);
    await refreshBasket();
  };

  const handleDeleteSoort = async (soort: string) => {
    await deleteOldestFruit(soort);
    await refreshBasket();
  };

  const handleClearBasket = async () => {
    if (confirm("Mand legen?")) {
      await deleteMand();
      await refreshBasket();
    }
  };

  if (loading) return <p>Bezig met laden...</p>;
  if (!mandDetail) return <p>Geen mandgegevens beschikbaar.</p>;

  return (
    <div>
      <h2>Fruitmand</h2>
      {mandDetail && (
        <>
          <p>Totaal fruit: <strong>{mandDetail.totaalAantal}</strong></p>
          <p>
            Status:{" "}
            <strong style={{ color: mandDetail.isBedorven ? "red" : "green" }}>
              {mandDetail.isBedorven ? "Bedorven" : "Vers"}
            </strong>
          </p>

          <div style={{ margin: "1rem 0" }}>
            <label>Bedorven fruit: {mandDetail.spoilagePercentage}%</label>
            <div style={{ height: "20px", background: "#eee", borderRadius: "10px", overflow: "hidden" }}>
              <div
                style={{
                  width: `${mandDetail.spoilagePercentage}%`,
                  background: mandDetail.isBedorven ? "red" : "green",
                  height: "100%",
                  transition: "width 0.3s"
                }}
              ></div>
            </div>
          </div>

          <ul>
            {Object.entries(oldestFruits).map(([soort, fruit]) => {
              const count = mandDetail.fruits.filter(f => f.soortFruit === soort).length;

              return (
                <li key={soort}>
                  <strong>{soort}</strong>: {count}
                  <button
                    onClick={() => handleDeleteSoort(soort)}
                    title={`Verwijder oudste ${soort}`}
                    style={{
                      marginLeft: "0.5rem",
                      background: "none",
                      border: "none",
                      cursor: "pointer",
                      color: "red",
                      fontSize: "1.2rem"
                    }}
                  >
                    🗑️
                  </button>
                  <div style={{ fontSize: "0.9em", marginTop: "0.25rem", marginLeft: "1rem" }}>
                    🕓 Oudste: {new Date(fruit.aankoopDatum).toLocaleDateString()}{" "}
                    {fruit.soortAppel && `(Soort: ${fruit.soortAppel})`}
                    {fruit.isBiologisch !== undefined &&
                      ` - ${fruit.isBiologisch ? "Biologisch" : "Niet biologisch"}`}
                    {" - "}
                    <span style={{ color: fruit.isBedorven ? "red" : "green" }}>
                      {fruit.isBedorven ? "Bedorven" : "Vers"}
                    </span>
                  </div>
                </li>
              );
            })}
          </ul>

          <div style={{ display: "flex", gap: "1rem", marginTop: "1rem" }}>
            <button onClick={handleDeleteOldestOverall}>🗑️ Verwijder oudste stuk fruit</button>
            <button onClick={handleClearBasket}>Mand legen</button>
          </div>
        </>
      )}
    </div>
  );
}
