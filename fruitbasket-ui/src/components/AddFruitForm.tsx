import { useState } from "react";
import { addFruit } from "../services/api";

export default function AddFruitForm({ onFruitAdded }: { onFruitAdded: () => void }) {
  const [soort, setSoort] = useState("");
  const [aankoopDatum, setAankoopDatum] = useState("");
  const [isBiologisch, setIsBiologisch] = useState(false);
  const [soortAppel, setSoortAppel] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    const payload: any = { aankoopDatum };

    if (soort === "banaan") payload.isBiologisch = isBiologisch;
    if (soort === "appel") payload.soortAppel = soortAppel;

    try {
        await addFruit(soort, payload);
        alert("Fruit toegevoegd!");
        onFruitAdded();
      } catch (err) {
        console.error("API error:", err);
        alert("Fout bij toevoegen fruit.");
      }
  };

  return (
    <form onSubmit={handleSubmit}>
      <input value={soort} onChange={(e) => setSoort(e.target.value)} placeholder="Fruitsoort" required />
      <input type="date" value={aankoopDatum} onChange={(e) => setAankoopDatum(e.target.value)} required />

      {soort === "banaan" && (
        <label>
          <input type="checkbox" checked={isBiologisch} onChange={(e) => setIsBiologisch(e.target.checked)} />
          Biologisch
        </label>
      )}

      {soort === "appel" && (
        <input value={soortAppel} onChange={(e) => setSoortAppel(e.target.value)} placeholder="Soort appel" />
      )}

      <button type="submit">Toevoegen</button>
    </form>
  );
}
