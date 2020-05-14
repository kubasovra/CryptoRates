using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CryptoRates.Data;
using Microsoft.AspNetCore.Identity;
using CryptoRates.Models;
using CryptoRates.Data.DTO;
using System.Security.Claims;

namespace CryptoRates.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PairsController : ControllerBase
    {
        private readonly CryptoContext _context;

        public PairsController(CryptoContext context)
        {
            _context = context;
        }

        // GET: api/Pairs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PairDTO>>> GetPairs()
        {
            //FIX USER
            /*
            var id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser currentUser = await _userManager.FindByIdAsync(id);
            List<PairDTO> pairDTOs = await _context.Pairs.Where(p => p.User == currentUser).Include(p => p.User).Include(p => p.FirstCurrency).Include(p => p.SecondCurrency).Select(p => PairToDTO(p)).ToListAsync();
            return pairDTOs;
            */
            return null;
        }

        // GET: api/Pairs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PairDTO>> GetPair(int id)
        {
            var pairDTO = PairToDTO(await _context.Pairs.FindAsync(id));

            if (pairDTO == null)
            {
                return NotFound();
            }

            return pairDTO;
        }

        // PUT: api/Pairs/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPair(int id, Pair pair)
        {
            if (id != pair.Id)
            {
                return BadRequest();
            }

            _context.Entry(pair).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PairExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pairs
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<PairDTO>> PostPair(PairDTO pairDTO)
        {
            //FIX USER
            /*
            if (pairDTO.FirstCurrency.Name == pairDTO.SecondCurrency.Name)
            {
                return NoContent();
            }
            var id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            ApplicationUser currentUser = await _userManager.FindByIdAsync(id);
            Pair newPair = PairFromDTO(pairDTO, currentUser, _context);
            _context.Pairs.Add(newPair);
            await _context.SaveChangesAsync();

            //PairFromDTO fills a pair with all needed data, such as links, and here we get it back
            pairDTO = PairToDTO(newPair);
            return pairDTO;
            */
            return null;
        }

        // DELETE: api/Pairs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeletePair(int id)
        {
            var pair = await _context.Pairs.FindAsync(id);
            if (pair == null)
            {
                return NotFound();
            }

            _context.Pairs.Remove(pair);
            await _context.SaveChangesAsync();

            return id;
        }

        private bool PairExists(int id)
        {
            return _context.Pairs.Any(e => e.Id == id);
        }

        private static PairDTO PairToDTO(Pair pair)
        {
            return new PairDTO(pair.Id, pair.User.Id.ToString())
            {
                FirstCurrency = new CurrencyDTO() {
                    CurrencyId = pair.FirstCurrency.Id,
                    Name = pair.FirstCurrency.Name,
                    Symbol = pair.FirstCurrency.Symbol,
                    ValueUSD = pair.FirstCurrency.ValueUSD,
                    WebPage = pair.FirstCurrency.WebPage,
                    ImageURL = pair.FirstCurrency.ImageURL
                },
                SecondCurrency = new CurrencyDTO() {
                    CurrencyId = pair.SecondCurrency.Id,
                    Name = pair.SecondCurrency.Name,
                    Symbol = pair.SecondCurrency.Symbol,
                    ValueUSD = pair.SecondCurrency.ValueUSD,
                    WebPage = pair.SecondCurrency.WebPage,
                    ImageURL = pair.SecondCurrency.ImageURL
                },
                State = pair.State,
                HistoricalData = pair.HistoricalData?.Select(double.Parse).ToList(),
                PriceFirstToSecond = pair.PriceFirstToSecond,
                PreviousPriceFirstToSecond = pair.PreviousPriceFirstToSecond,
                TargetPrice = pair.TargetPrice,
                IsNotifyOnPrice = pair.IsNotifyOnPrice
            };
        }

        private static Pair PairFromDTO(PairDTO pairDTO, AppUser user, CryptoContext context )
        {
            return new Pair()
            {
                User = user,
                FirstCurrency = context.Currencies.FirstOrDefault(c => c.Name.ToLower() == pairDTO.FirstCurrency.Name.ToLower()),
                SecondCurrency = context.Currencies.FirstOrDefault(c => c.Name.ToLower() == pairDTO.SecondCurrency.Name.ToLower()),
                State = pairDTO.State,
                HistoricalData = pairDTO.HistoricalData?.Select(p => p.ToString()).ToList(),
                PriceFirstToSecond = pairDTO.PriceFirstToSecond,
                PreviousPriceFirstToSecond = pairDTO.PreviousPriceFirstToSecond,
                TargetPrice = pairDTO.TargetPrice,
                IsNotifyOnPrice = pairDTO.IsNotifyOnPrice
            };
        }
    }
}
