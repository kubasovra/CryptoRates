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
        private readonly UserManager<ApplicationUser> _userManager;

        public PairsController(CryptoContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Pairs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PairDTO>>> GetPairs()
        {
            var id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser currentUser = await _userManager.FindByIdAsync(id);
            List<PairDTO> pairDTOs = await _context.Pairs.Where(p => p.User == currentUser).Include(p => p.User).Include(p => p.FirstCurrency).Include(p => p.SecondCurrency).Select(p => PairToDTO(p)).ToListAsync();
            return pairDTOs;
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
            if (id != pair.PairId)
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
            var id = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApplicationUser currentUser = await _userManager.FindByIdAsync(id);
            Pair newPair = PairFromDTO(pairDTO, currentUser, _context);
            _context.Pairs.Add(newPair);
            await _context.SaveChangesAsync();

            //PairFromDTO fills a pair with all needed data, such as links, and here we get it back
            pairDTO = PairToDTO(newPair);
            return pairDTO;
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
            return _context.Pairs.Any(e => e.PairId == id);
        }

        private static PairDTO PairToDTO(Pair pair)
        {
            return new PairDTO(pair.PairId, pair.User.Id)
            {
                FirstCurrency = new CurrencyDTO() {
                    CurrencyId = pair.FirstCurrency.CurrencyId,
                    Name = pair.FirstCurrency.Name,
                    Symbol = pair.FirstCurrency.Symbol,
                    ValueUSD = pair.FirstCurrency.ValueUSD,
                    WebPage = pair.FirstCurrency.WebPage,
                    ImageURL = pair.FirstCurrency.ImageURL
                },
                SecondCurrency = new CurrencyDTO() {
                    CurrencyId = pair.SecondCurrency.CurrencyId,
                    Name = pair.SecondCurrency.Name,
                    Symbol = pair.SecondCurrency.Symbol,
                    ValueUSD = pair.SecondCurrency.ValueUSD,
                    WebPage = pair.SecondCurrency.WebPage,
                    ImageURL = pair.SecondCurrency.ImageURL
                },
                PriceFirstToSecond = pair.PriceFirstToSecond,
                PreviousPriceFirstToSecond = pair.PreviousPriceFirstToSecond,
                TargetPrice = pair.TargetPrice,
                TargetPriceAbsoluteChange = pair.TargetPriceAbsoluteChange,
                TargetPricePercentChange = pair.TargetPricePercentChange,
                IsNotifyOnPrice = pair.IsNotifyOnPrice,
                IsNotifyOnAbsoluteChange = pair.IsNotifyOnAbsoluteChange,
                IsNotifyOnPercentChange = pair.IsNotifyOnPercentChange
            };
        }

        private static Pair PairFromDTO(PairDTO pairDTO, ApplicationUser currentUser, CryptoContext context )
        {
            return new Pair()
            {
                User = currentUser,
                FirstCurrency = context.Currencies.FirstOrDefault(c => c.Name.ToLower() == pairDTO.FirstCurrency.Name.ToLower()),
                SecondCurrency = context.Currencies.FirstOrDefault(c => c.Name.ToLower() == pairDTO.SecondCurrency.Name.ToLower()),
                PriceFirstToSecond = pairDTO.PriceFirstToSecond,
                PreviousPriceFirstToSecond = pairDTO.PreviousPriceFirstToSecond,
                TargetPrice = pairDTO.TargetPrice,
                TargetPriceAbsoluteChange = pairDTO.TargetPriceAbsoluteChange,
                TargetPricePercentChange = pairDTO.TargetPricePercentChange,
                IsNotifyOnPrice = pairDTO.IsNotifyOnPrice,
                IsNotifyOnAbsoluteChange = pairDTO.IsNotifyOnAbsoluteChange,
                IsNotifyOnPercentChange = pairDTO.IsNotifyOnPercentChange
            };
        }
    }
}
